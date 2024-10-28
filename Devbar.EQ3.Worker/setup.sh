#!/bin/sh
USER=pi
TITLE="EQ3 MQTT daemon"

WORKINGDIR=`pwd`
DAEMON="eq3-to-mqtt"
EXECUTE="$WORKINGDIR/Devbar.EQ3.Worker"

sudo mkdir /etc/eq3
sudo cp $WORKINGDIR/appsettings.json /etc/eq3/config.json

sudo adduser --system --no-create-home --group $USER

SERVICEFILE="$WORKINGDIR/systemd/$DAEMON.service"
sudo cp $WORKINGDIR/systemd/template.service $SERVICEFILE
sudo sed -i s/"TITLE"/"$TITLE"/g $SERVICEFILE
sudo sed -i s/"USER"/$USER/g $SERVICEFILE
sudo sed -i s@"WORKINGDIR"@"$WORKINGDIR"@g $SERVICEFILE
sudo sed -i s@"EXECUTE"@"$EXECUTE"@g $SERVICEFILE

sudo cp $SERVICEFILE /lib/systemd/system/$DAEMON.service
sudo ln -s $WORKINGDIR/systemd/$DAEMON.service /etc/systemd/system/$DAEMON.service
sudo ln -s $WORKINGDIR/systemd/$DAEMON.service /etc/systemd/system/multi-user.target.wants/$DAEMON.service

sudo systemctl daemon-reload
sudo systemctl enable $DAEMON