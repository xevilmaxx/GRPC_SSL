# GRPC SSL

## Generic Tutorial about OpenSSL:

https://blog.devolutions.net/2020/07/tutorial-how-to-generate-secure-self-signed-server-and-client-certificates-with-openssl

## 1) Install Chocolatey in Windows 10: (on Administrative Powershell)

https://theknowledgehound.home.blog/2020/03/05/how-to-install-chocolatey-on-windows-10/

```powershell
Set-ExecutionPolicy Bypass -Scope Process -Force; `
  iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
```

## 2) Install OpenSSL

https://adamtheautomator.com/openssl-windows-10/

```powershell
choco install OpenSSL.Light
```

Usual installation directory: '**C:\Program Files\OpenSSL**\'

## 3) SSL certificate generation and C#

https://stackoverflow.com/questions/37714558/how-to-enable-server-side-ssl-for-grpc

```bash
@echo off
rem certificate Expiration: 3650 days -> 10 years

SET OPENSSL_CONF=C:\Program Files\OpenSSL\bin\openssl.cnf
SET "password=1111"
SET "certValidityDays=365"
SET "desLength=4096"

rem ---------------------
rem Generate Basic Key
rem ---------------------
echo Generate CA key:
openssl genrsa -passout pass:%password% -des3 -out ca.key %desLength%

echo Generate CA certificate:
openssl req -passin pass:%password% -new -x509 -days %certValidityDays% -key ca.key -out ca.crt -subj  "/C=IT/ST=MB/L=Monza/O=SwLink/OU=ParkO/CN=MyRootCA"


rem ---------------------
rem Server Key Generation
rem ---------------------
echo Generate server key:
openssl genrsa -passout pass:%password% -des3 -out server.key %desLength%

echo Generate server signing request:
openssl req -passin pass:%password% -new -key server.key -out server.csr -config config.conf

echo Self-sign server certificate:
openssl x509 -req -passin pass:%password% -days %certValidityDays% -in server.csr -CA ca.crt -CAkey ca.key -set_serial 01 -out server.crt -extensions ext -extfile config.conf

echo Remove passphrase from server key:
openssl rsa -passin pass:%password% -in server.key -out server.key

rem ---------------------
rem Client Key Generation
rem ---------------------
echo Generate client key
openssl genrsa -passout pass:%password% -des3 -out client.key %desLength%

echo Generate client signing request:
openssl req -passin pass:%password% -new -key client.key -out client.csr -config config.conf

echo Self-sign client certificate:
openssl x509 -passin pass:%password% -req -days %certValidityDays% -in client.csr -CA ca.crt -CAkey ca.key -set_serial 01 -out client.crt -extensions ext -extfile config.conf

echo Remove passphrase from client key:
openssl rsa -passin pass:%password% -in client.key -out client.key

pause
```



Another Sample:

https://github.com/perhallgren/grpc/tree/TLSSupport/examples/csharp/helloworld-from-cli



### Missing OpenSSL "openssl.cnf"?

https://adamtheautomator.com/openssl-windows-10/

1) Open Powershell with Administrative rights

```powershell
//Move to correct folder of your OpenSSL installation
cd "C:\Program Files\OpenSSL\bin"

//Fetch cnf file from Web
Invoke-WebRequest 'http://web.mit.edu/crypto/openssl.cnf' -OutFile .\openssl.cnf
```

2) Now you might attempt to re-run SSL generation script

**Might also need this if there are still errors in generation:**

https://stackoverflow.com/questions/63893662/cant-load-root-rnd-into-rng

**Try** removing or **commenting** `RANDFILE = $ENV::HOME/.rnd` line in `/etc/ssl/openssl.cnf` | `C:\Program Files\OpenSSL\bin\openssl.cnf`

**Also you might need to execute SSLGenerate.bat as Administrator**

### Additional Config for SSL generation (config.conf)

https://medium.com/@arkadybalaba/quick-run-to-secure-your-grpc-api-with-ssl-tls-fbd910ec8eee#758f

```
[req]
prompt = no
req_extensions = ext
distinguished_name = req_distinguished_named

[ ext ]
subjectAltName = IP:0.0.0.0, IP:127.0.0.1

[ req_distinguished_named ]
C=IT
ST=MB
L=Monza
O=SwLink
OU=ParkO
CN=localhost
```

This config is Universal.

Its ok for Client and Server, when you want connect through BloomRPC you will specify 127.0.0.1 as Host.

Once generated its enough just copy paste certificates across all microservices.

![](C:\Users\maxym.oboyshev\source\repos\GRPC_SSL\images\BloomRPC_SSL_configuration.JPG)

