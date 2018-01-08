with import <nixpkgs> {}; rec {
  frameworkVersion = "4.5";
  fsharpVersion = "4.0";
  assemblyVersion = "4.4.0.0";

  fsharpProbeEnv = stdenv.mkDerivation {
      name = "FsharpProbe";
      buildInputs = [ mesa mono monodevelop dotnetPackages.Nuget xorg.libX11 fsharp SDL2 ];
      LD_LIBRARY_PATH="${xorg.libX11}/lib:${mesa}/lib:${SDL2}/lib";
      FSharpTargetsPath="${fsharp}/lib/mono/${frameworkVersion}/Microsoft.FSharp.Targets";
      MONO_PATH="${fsharp}/lib/mono/Reference\ Assemblies/Microsoft/FSharp/.NETFramework/v${fsharpVersion}/${assemblyVersion}/";
  };
}
