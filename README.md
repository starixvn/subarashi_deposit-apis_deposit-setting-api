# Deposit Setting API

The Deposit Setting API is a web API for handling deposit settings and related tasks.

## Documents

- [Deposit Setting API](https://docs.google.com/document/d/1eAesvesTwdyhaJBGplc5BPgsTD_deoiZVK4Bw5SKFRk/edit?usp=sharing)

## Notes

- [Apply Conventional Commits](https://gitlab.nexdev.net/nexdev/documentation/-/blob/master/Apply%20Conventional%20Commits.md)

## Built with

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Run Load Tests

- [Install K6](https://k6.io/docs/get-started/installation)
- Go to test folder
```shell
cd tests/LoadTests
```
- Run load tests
```shell
$env:K6_WEB_DASHBOARD="true" ; $env:K6_WEB_DASHBOARD_EXPORT="./Reports/[file-test-name].html" ; k6 run [file-test-name].js

or

K6_WEB_DASHBOARD=true K6_WEB_DASHBOARD_EXPORT="./Reports/[file-test-name].html" k6 run [file-test-name].js
```