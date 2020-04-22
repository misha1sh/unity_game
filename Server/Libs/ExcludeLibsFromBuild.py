import os

for root, subdirs, files in os.walk("./"):
    for file in files:
        if not file.endswith(".cs"): continue
        
        with open(root + "/" + file) as f:
            inp = f.read()
        
        if inp.startswith("#if UNITY_EDITOR"): continue
        
        inp = "#if UNITY_EDITOR\n" + inp + "\n#endif"
        
        with open(root + "/" + file, "w") as f:
            f.write(inp)
            
