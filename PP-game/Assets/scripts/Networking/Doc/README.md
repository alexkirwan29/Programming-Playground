# PP.Networking documentation

## Directory and Namespace hierarchy
The directory hierarchy should match the namespaces.

#### PP.Networking
Is where the shared code goes. (code for both the server and client)

#### PP.Networking.Packets
For the packets that are shared between the server and clients. These packets will be directly used in many scripts and should in their own namespace to keep things tidy.

#### PP.Networking.Server
All server **ONLY** code. Eventually this whole namespace will be excluded from the client builds.

#### PP.Networking.Client
All client **ONLY** code. Eventually this whole namespace will be excluded from the server builds.