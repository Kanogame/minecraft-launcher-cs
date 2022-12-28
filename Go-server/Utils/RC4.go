package Utils

import (
	"encoding/base64"
)

func Encrypt(pwd []byte, data []byte) []byte {
	var a, i, j, k, tmp int

	var key = make([]int, 256)
	var box = make([]int, 256)
	var cipher = make([]byte, len(data))

	for i = 0; i < 256; i++ {
		key[i] = int(pwd[i%len(pwd)])
		box[i] = i
	}
	i = 0
	for j = 0; i < 256; i++ {
		j = (j + box[i] + key[i]) % 256
		tmp = box[i]
		box[i] = box[j]
		box[j] = tmp
	}
	i = 0
	j = 0
	for a = 0; i < len(data); i++ {
		a++
		a %= 256
		j += box[a]
		j %= 256
		tmp = box[a]
		box[a] = box[j]
		box[j] = tmp
		k = box[((box[a] + box[j]) % 256)]
		cipher[i] = byte(int(data[i]) ^ k)
	}
	return cipher
}

func DecryptPW(passwd string, key string) string {
	data, err := base64.StdEncoding.DecodeString(passwd)
	if err != nil {
		panic(err)
	}
	return string(Encrypt([]byte(key), data))
}
