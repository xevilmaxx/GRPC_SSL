@echo off
rem certificate Expiration: 365 days -> 1 year

SET OPENSSL_CONF=C:\Program Files\OpenSSL\bin\openssl.cnf
SET "password=1111"
SET "certValidityDays=365"
SET "desLength=4096"


echo ##########################
echo Generate Root Certificate
echo ##########################
echo.
echo Generate CA key:
openssl genrsa -passout pass:%password% -des3 -out ca.key %desLength%

echo Generate CA certificate:
openssl req -passin pass:%password% -new -x509 -days %certValidityDays% -key ca.key -out ca.crt -subj  "/C=IT/ST=MB/L=Monza/O=SwLink/OU=test/CN=MyRootCA"

echo.
echo.

echo ##########################
echo Server Key Generation
echo ##########################
echo.
echo Generate server key:
openssl genrsa -passout pass:%password% -des3 -out server.key %desLength%

echo Generate server signing request:
openssl req -passin pass:%password% -new -key server.key -out server.csr -config config.conf

echo Self-sign server certificate:
openssl x509 -req -passin pass:%password% -days %certValidityDays% -in server.csr -CA ca.crt -CAkey ca.key -set_serial 01 -out server.crt -extensions ext -extfile config.conf

echo Remove passphrase from server key:
openssl rsa -passin pass:%password% -in server.key -out server.key

echo.
echo.

echo ##########################
echo Client Key Generation
echo ##########################
echo.
echo Generate client key
openssl genrsa -passout pass:%password% -des3 -out client.key %desLength%

echo Generate client signing request:
openssl req -passin pass:%password% -new -key client.key -out client.csr -config config.conf

echo Self-sign client certificate:
openssl x509 -passin pass:%password% -req -days %certValidityDays% -in client.csr -CA ca.crt -CAkey ca.key -set_serial 01 -out client.crt -extensions ext -extfile config.conf

echo Remove passphrase from client key:
openssl rsa -passin pass:%password% -in client.key -out client.key

echo.
echo.

echo ##########################
echo Cleaning useless files:
echo ##########################
echo.
del "*.csr"
del "ca.key"
echo DONE

pause
