#!/usr/bin/env bash
sed -e 's/\([0-9]\+\)-\([0-9]\+\) \([a-z]\): \(.*\)/echo .\4 | sed -e "s\/^.\\{\1\\}\3.*$\/\&!\/" | sed -e "s\/^.\\{\2\\}\3.*$\/\&!\/"/' $1 