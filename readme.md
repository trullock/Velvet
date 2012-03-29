# Development DNS Server

Resolves all lookups for any hostname matching a given regex to `127.0.0.1`

No more host file editing!

Win.


## Usage

Change your primary DNS server to be `127.0.0.1`, and your secondary DNS server to whatever your primary one was.
If there is no explicit primary DNS server, run `ipconfig /all` to find out what your DNS server is.

Run `install.bat`

Go to Services and start `Dev Dns Server`