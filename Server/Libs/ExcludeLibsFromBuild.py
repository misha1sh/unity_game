import os

for root, subdirs, files in os.walk("./"):
    for file in files:
        if not file.endswith(".cs"): continue

        with open(root + "/" + file) as f:
            inp = f.read()

        if inp.startswith("#if UNITY_EDITOR || !UNITY_WEBGL\n"): continue
        inp = inp.replace("#if UNITY_EDITOR\n", "#if UNITY_EDITOR || !UNITY_WEBGL\n")
        # inp = "#if UNITY_EDITOR || !UNITY_WEBGL\n" + inp + "\n#endif"

        with open(root + "/" + file, "w") as f:
            f.write(inp)

