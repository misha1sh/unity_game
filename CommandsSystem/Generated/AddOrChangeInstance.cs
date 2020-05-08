
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

namespace CommandsSystem.Commands {
    public partial class AddOrChangeInstance : ICommand  {

        public AddOrChangeInstance(){}
        
        public AddOrChangeInstance(Instance instance) {
            this.instance = instance;
        }

        private byte[] SerializeLittleEndian() {
            unsafe {
var arr = new byte[37];
arr[0] = (byte)(instance.id & 0x000000ff);
   arr[1] = (byte)((instance.id & 0x0000ff00) >> 8);
   arr[2] = (byte)((instance.id & 0x00ff0000) >> 16);
   arr[3] = (byte)((instance.id & 0xff000000) >> 24);

var bytes_instance_name= Encoding.UTF8.GetBytes(instance.name);
    Assert.IsTrue(bytes_instance_name.Length <= 25);

arr[4] = (byte)(bytes_instance_name.Length & 0x000000ff);
   arr[5] = (byte)((bytes_instance_name.Length & 0x0000ff00) >> 8);
   arr[6] = (byte)((bytes_instance_name.Length & 0x00ff0000) >> 16);
   arr[7] = (byte)((bytes_instance_name.Length & 0xff000000) >> 24);
    Array.Copy(bytes_instance_name ,0, arr, 8, bytes_instance_name.Length);

arr[33] = (byte)(instance.currentLoadedGamemodeNum & 0x000000ff);
   arr[34] = (byte)((instance.currentLoadedGamemodeNum & 0x0000ff00) >> 8);
   arr[35] = (byte)((instance.currentLoadedGamemodeNum & 0x00ff0000) >> 16);
   arr[36] = (byte)((instance.currentLoadedGamemodeNum & 0xff000000) >> 24);



                return arr;
            }

        }
        
        public byte[] Serialize() {
            if (BitConverter.IsLittleEndian)
                return SerializeLittleEndian();
            throw new Exception("BigEndian not supported");
        }
        
        private static AddOrChangeInstance DeserializeLittleEndian(byte[] arr) {
            var result = new AddOrChangeInstance();
            Assert.AreEqual(arr.Length, 37);
            unsafe {
result.instance = new Instance();
result.instance.id = (arr[0] | (arr[1] << 8) | (arr[2] << 16) | (arr[3] << 24));

int len_result_instance_name;
len_result_instance_name = (arr[4] | (arr[5] << 8) | (arr[6] << 16) | (arr[7] << 24));
result.instance.name = Encoding.UTF8.GetString(arr, 8, len_result_instance_name);

result.instance.currentLoadedGamemodeNum = (arr[33] | (arr[34] << 8) | (arr[35] << 16) | (arr[36] << 24));


             
                return result;
            }

        }
        
        public static AddOrChangeInstance Deserialize(byte[] arr) {
            if (BitConverter.IsLittleEndian)
                return DeserializeLittleEndian(arr);
            throw new Exception("BigEndian not supported");
        }
        
        
        public string AsJson() {
            return $"{{'instance':{instance}}}";
        }
        
        public override string ToString() {
            return "AddOrChangeInstance " + AsJson();
        }
    }
}