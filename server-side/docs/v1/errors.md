# API Errors

## General errors (F)

| Code	| HTTP  | Reason																													|
|:-----:|:-----:|-----------------------------------------------------------------|
|0xFFFF |	all		| Unknown error				|																						|
|0x0000	|	all		|	No Error																												|
|0xF404	|	404		|	Not Found																												|
|0xF500	|	5XX		|	Server Error																										|

## Auth Errors (A)

| Code	| HTTP  | Reason																													|
|:-----:|:-----:|-----------------------------------------------------------------|
|0xA000	|	400		|	Invalid Request, Oauth Token not valid.													|
|0xA001	|	401		|	Not Authorized, The user does not have Authorization.						|