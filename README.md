# gateway
.net using aws api gateway with IAM user and HTTP Signature

## app config points to a credential file that must contain
[{profileName}]
aws_access_key_id = {accessKeyId}
aws_secret_access_key = {secretAccesskeyId}

see https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html


The project refs are pointing to a local build of the AWS SDK to help debugging. Eventually this arrangement will be replaced with packages.