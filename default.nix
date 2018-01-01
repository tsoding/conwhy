with import <nixpkgs> {}; rec {
  fsharpVersion = "4.0";
  assemblyVersion = "4.4.0.0";

  fsharpProbeEnv = stdenv.mkDerivation {
      name = "FsharpProbe";
      buildInputs = [ mesa mono46 dotnetPackages.Nuget xorg.libX11 fsharp ];
      LD_LIBRARY_PATH="${xorg.libX11}/lib:${mesa}/lib";
      MONO_PATH="${fsharp}/lib/mono/Reference Assemblies/Microsoft/FSharp/.NETFramework/v${fsharpVersion}/${assemblyVersion}/";
  };
}
