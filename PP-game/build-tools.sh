#!/bin/bash
TERM="xterm"

#build-tools.sh server run
#build-tools.sh server stop

function usage()
{
  echo "Usage: build-tools [server|client] [stop|start]"
  exit 2
}

# Check the mode is correct.
if [ "$2" == "stop" ]; then
  MODE="stop"
elif [ "$2" == "start" ]; then
  MODE="start"
else
  usage
fi

cd ./builds

if [ "$1" == "server" ]; then
  echo "server $MODE"
  if [ "$MODE" == "start" ]; then
    gnome-terminal -- ./run-server.sh
  elif [ "$MODE" == "stop" ]; then
    killall run-server.sh
  fi
elif [ "$1" == "client" ]; then
  echo "client $MODE"
  if [ "$MODE" == "start" ]; then
    ./pp-game&
  elif [ "$MODE" == "stop" ]; then
    killall pp-game
  fi
else
  usage
fi