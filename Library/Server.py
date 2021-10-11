#!/usr/bin/python2

import BaseHTTPServer, SimpleHTTPServer
import ssl

print('Basic HTTPS server. It should be run with Python2 (not 3)')
httpd = BaseHTTPServer.HTTPServer(('127.0.0.1', 4443), SimpleHTTPServer.SimpleHTTPRequestHandler)

print('OK buddy now open http://127.0.0.1:4443 in your web browser, and accept the camera (icon at the right of the URL bar)')
httpd.serve_forever()