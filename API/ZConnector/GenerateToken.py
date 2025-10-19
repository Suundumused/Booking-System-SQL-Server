import os
import base64

# Generate a 256-bit (32-byte) key
print(base64.urlsafe_b64encode(os.urandom(32)).decode('utf-8'))