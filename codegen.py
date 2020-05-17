import os
import re
import random

def ser_int(name, offset):
    return """arr[""" + str(offset) +     """] = (byte)("""+name+""" & 0x000000ff);
   arr[""" + str(offset + 1) + """] = (byte)(("""+name+""" & 0x0000ff00) >> 8);
   arr[""" + str(offset + 2) + """] = (byte)(("""+name+""" & 0x00ff0000) >> 16);
   arr[""" + str(offset + 3) + """] = (byte)(("""+name+""" & 0xff000000) >> 24);
"""


def deser_int(name, offset):
    return name+" = (arr["+str(offset)+"] | (arr["+str(offset+1)+"] << 8) | (arr["+str(offset+2)+"] << 16) | (arr["+str(offset+3)+"] << 24));\n"

def pack_int(name, offset):
    return ser_int(name, offset), deser_int("result."+name, offset), offset + 4


def ser_float(name, offset):
    nname = name.replace(".", "_")# +"_" + str(random.randint(0, 10000))
    f_name = "f_" + nname
    i_name = "i_" + nname
    return "    float "+f_name+" = " + name + ";\n" + \
           "    int "+i_name+" = *((int*)&"+f_name+");\n" + \
                ser_int(i_name, offset)

def deser_float(name, offset):
    nname = name.replace(".", "_") #+"_" + str(random.randint(0, 10000))
    f_name = "f_" + nname
    i_name = "i_" + nname
    
    return "int " + i_name +";\n" + \
           deser_int(i_name, offset) + \
           "float " + f_name + " = *((float*)&"+i_name+");\n" + \
           name + " = " + f_name + ";\n"

def pack_float(name, offset):
    return ser_float(name, offset), deser_float("result."+name, offset), offset + 4


def ser_bool(name, offset):
    return "arr[" + str(offset) +  "] = ("+name+"?(byte)1: (byte)0);\n"

def deser_bool(name, offset):
    return name+" = (arr["+str(offset)+"] == 1);\n"

def pack_bool(name, offset):
    return ser_bool(name, offset), deser_bool("result."+name, offset), offset + 1

MAX_STR_LEN = 25
def ser_string(name, offset):
    nname = "bytes_" + name.replace(".", "_")    
    return """var """ + nname + """= Encoding.UTF8.GetBytes("""+name+""");
    Assert.IsTrue("""+nname +".Length <= " + str(MAX_STR_LEN) + """);

""" + ser_int(nname + ".Length", offset) + \
"""    Array.Copy("""+nname+""" ,0, arr, """+str(offset+4)+""", """+nname+""".Length);
"""

def deser_string(name, offset):
    nname = "len_" + name.replace(".", "_")
    return "int "+nname+";\n" + \
            deser_int(nname, offset) + \
            name + " = Encoding.UTF8.GetString(arr, "+str(offset + 4)+", "+nname+");\n"

def pack_string(name, offset):
    return ser_string(name, offset), deser_string("result."+name, offset), offset + MAX_STR_LEN + 4


#def auto_pack_json(func):
#    def ret(name, offset):
#        s, d, o = func(name, offset)
#        return s, d


def extract_fields(filename):
    with open(filename, encoding="utf-8") as file: inp = file.read()
    classname = filename.split("/")[-1].split(".")[0]
    
    inp = inp.split(classname, 1)[1].split("{")[1]
    fields = []
    for line in inp.split("\n"):
        if re.match("\s*public [_A-Za-z0-9]+ [_A-Za-z0-9]+\s*=*\s*[0-9\.a-zA-Z]*[;\s]*$", line):
            ll = line.split()
            public, typ, name = ll[0], ll[1], ll[2]
            name = name.split(";")[0]
            fields.append((typ, name))
            
        if re.match("\s*\/\/\/\s*public [_A-Za-z0-9]+ [_A-Za-z0-9]+\s*=*\s*[0-9\.a-zA-Z]*[;\s]*$", line):
            ll = line.split()
            public, typ, name = ll[1], ll[2], ll[3]
            name = name.split(";")[0]
            fields.append((typ, name))
    
    print(filename,  fields)        
    return fields




def ser_field(typ, name, offset):
    if typ not in packers:
        print("unknown type: ", typ)
        return "", "", offset
    
    s, d, o = packers[typ](name, offset)
    s += "\n"
    d += "\n"
    return s, d, o
    


def pack_custom_type(typename):
    def pack_custom(stname, offset):
        ser, deser = "", "" #, json, jsser, jsdeser = "", "", "", "", ""
        deser += "result." + stname + " = new " + typename + "();\n";
    
        for (typ, name) in extract_fields(paths[typename]):
            s, d, o = ser_field(typ, stname + "." + name, offset)
            ser += s
            deser += d
            offset = o
        return ser, deser, offset
            
    return pack_custom


paths = {
    "PlayerAnimationState": "Character/PlayerAnimationState.cs",
    "Vector3": "Util2/Vector3.txt",
    "Quaternion": "Util2/Quaternion.txt",
    "HPChange": "Character/HP/HPChange.cs",
    "PlayerProperty": "Interpolation/Properties/PlayerProperty.cs",
    "Player": "Game/Player.cs",
    "Instance": "Game/Instance.cs",
    "MatchInfo": "Game/MatchInfo.cs",
    "SpawnPrefabCommand": "CommandsSystem/Commands/SpawnPrefabCommand.cs"
}

packers = {
    "int": pack_int,
    "float": pack_float,
    "bool": pack_bool,
    "string": pack_string,
    "PlayerAnimationState": pack_custom_type("PlayerAnimationState"),
    "Vector3": pack_custom_type("Vector3"),
    "Quaternion": pack_custom_type("Quaternion"),
    "HPChange": pack_custom_type("HPChange"),
    "PlayerProperty": pack_custom_type("PlayerProperty"),
    "Player": pack_custom_type("Player"),
    "Instance": pack_custom_type("Instance"),
    "SpawnPrefabCommand": pack_custom_type("SpawnPrefabCommand"),
}

commands = []

files = os.listdir("CommandsSystem/Commands")
ffiles = []
for i in files:
    ffiles.append((i, "CommandsSystem/Commands/" + i))
    
for filename, path in ffiles + [("Pistol.cs", "Character/Guns/Pistol.cs"),
                                ("ShotGun.cs", "Character/Guns/ShotGun.cs"),
                                ("SemiautoGun.cs", "Character/Guns/SemiautoGun.cs"),
                                ("BombGun.cs", "Character/Guns/BombGun.cs")]:
    if not filename.endswith(".cs"): continue
    
    with open(path, encoding="utf-8") as file: inp = file.read()
    
    command = filename.split(".")[0]
    commands.append(command)

    namespace = inp.split("namespace")[1].split("{")[0].replace(" ", "")
    #inp = inp.split(command, 1)[1].split("{")[1]

    serialize = ""
    deserialize = ""
    json = ""
    offset = 0
    constr_args = ""
    constr = ""
    
    
    for (typ, name) in extract_fields(path):
            
            s, d, o = ser_field(typ, name, offset)
            
            serialize += s
            deserialize += d
            offset = o
           
            json += "'" + name + "':" + "{"  + name + "},"
            constr_args += typ + " " + name + ","
            constr += "this." + name + " = " + name + ";\n"
            
    json = json[:-1]
    constr = constr[:-1]
    constr_args = constr_args[:-1]
    
    serialize = "var arr = new byte[" + str(offset) +  "];\n" + serialize

    out = """
using System;
using System.Text;
using Character;
using Interpolation;
using Interpolation.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using Character.HP;
using CommandsSystem;
using GameMode;

namespace """+namespace+""" {
    public partial class """ + command + """ : ICommand  {

        public """ + command + """(){}
        
        public """ + command + """(""" + constr_args + """) {
            """ + constr + """
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
""" + serialize + """
                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static """ + command + """ DeserializeLittleEndian(byte[] arr) {
            var result = new """ + command +"""();
            Assert.AreEqual(arr.Length, """ + str(offset) + """);
            unsafe {
""" + deserialize + """             
                return result;
            }

        }
        
        public static """ + command + """ Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{""" + json + """}}";
        }
        
        public override string ToString() {
            return \"""" + command + """ \" + AsJson();
        }
    }
}"""
    with open("CommandsSystem/Generated/" + filename, "w", encoding="utf-8") as file:
        file.write(out)
        

commands_system_ser = ""
commands_system_deser = ""

for i in range(len(commands)):
    c = commands[i].lower()
    commands_system_ser +=    """            if (command is """+commands[i] +" " +c+""") {
                stream.WriteByte((byte)"""+str(i)+""");
                var buf = """+c+""".Serialize();
                stream.Write(buf, 0, buf.Length);
            } else
"""
    
    commands_system_deser += """             case """ + str(i) + """:
                     return """ + commands[i] + """.Deserialize(arr);
    """

with open("CommandsSystem/CommandsSystem.cs", "r", encoding="utf-8") as file:
    commands_syst = file.read()

commands_syst = re.sub("BEGIN2[^щ]*END2", "BEGIN2*/\n" + commands_system_ser + "/*END2", commands_syst)
commands_syst = re.sub("BEGIN1[^щ]*END1", "BEGIN1*/\n" + commands_system_deser + "/*END1", commands_syst)

with open("CommandsSystem/CommandsSystem.cs", "w") as file:
    file.write(commands_syst)