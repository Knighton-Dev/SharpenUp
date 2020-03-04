# Environment Setup

This is how I setup the environment to run the XUnit test. 

## Example Environment  

I've included a file called `exampleEnv.sh` in the project. This file has the export statements needed for the tests to run. Obviously, you'll need to plug in your own credentials and information, but this should get you started. 

On most Unix based machines, you should be able to set these variables by using the command:

```console
source ./exampleEnv.sh
```

The file sets the following environment variables:
```console
export GOOD_API_KEY="YOUR_API_KEY"
export ACCOUNT_EMAIL="YOUR_EMAIL"
export GOOD_MONITOR_ID_1=A_GOOD_ID
export GOOD_MONITOR_ID_2=A_GOOD_ID
export GOOD_MONITOR_1_FRIENDLY_NAME="A_GOOD_NAME"
export GOOD_MONITOR_2_FRIENDLY_NAME="A_GOOD_NAME"
export GOOD_MONITOR_1_URL="A_GOOD_URL"
export GOOD_MONITOR_2_URL="A_GOOD_URL"
export PSP_NAME_1="A_GOOD_PSP"
export PSP_NAME_2="A_GOOD_PSP"
export PSP_URL_1="A_GOOD_PSP_URL"
export PSP_URL_2="A_GOOD_PSP_URL"
```