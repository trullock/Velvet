# Development DNS Server

Are you fed up with maintaining a host file that looks like this:

<pre>
127.0.0.1 SiteA
127.0.0.1 SiteB
127.0.0.1 SiteC
127.0.0.1 SiteD
</pre>

By using a convention such as a top level domain of `.dev`, this simple DNS Server will answer all lookups for those addresses with `127.0.0.1`

This means you can access `anything.dev` and it will resolve to your machine. Much like `*.mymachine.me`, but without the prerequisite of internet access.

## Configuration

Edit the regex in the `app.config` to match your particular local dev hostname pattern.

## Usage

Change your primary DNS server to be `127.0.0.1`, and your secondary DNS server to whatever your primary one was.
If there is no explicit primary DNS server, run `ipconfig /all` to find out what your DNS server is.

Run `install.bat`

Go to Services and start `Dev Dns Server`