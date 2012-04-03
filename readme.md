# Velvet DNS Server

A simple DNS server that adds pattern matching functionality to your standard hosts file.

## Why

Are you fed up with maintaining a host file that looks like this:

<pre>
127.0.0.1 SiteA
127.0.0.1 SiteB
127.0.0.1 SiteC
127.0.0.1 SiteD
</pre>

By using a convention such as a top level domain of `.dev`, you can answer all lookups for those names with `127.0.0.1`

This means you can access `anything.dev` and it will resolve to your machine. Much like `*.mymachine.me`, but without the prerequisite of internet access.

## Configuration

### Mappings 

Edit your host file as you normally would, except the additional syntax is now supported:

<pre>
&lt;IP|Name&gt; &lt;A|C&gt; &lt;Name pattern&gt;
</pre>

for example, to map `anything.dev` to `127.0.0.1`:

<pre>
127.0.0.1 A *.dev
</pre>

to map `sausages.com` to `bacon.com`:

<pre>
bacon.com C sausages\.com
</pre>

### Hosts file

If you don't want to use your hosts file to configure Velvet, set the `HostsPath` setting in the `app.config` to a different path. The path can be relative to the Velvet executable or absolute.

### Limitations

Currently, only IPv4 A and CName records are supported.

## Usage

Change your primary DNS server to be `127.0.0.1`, and your secondary DNS server to whatever your primary one was.
If there is no explicit primary DNS server, run `ipconfig /all` to find out what your DNS server is.

Run `install.bat`

Go to Services and start `Velvet`