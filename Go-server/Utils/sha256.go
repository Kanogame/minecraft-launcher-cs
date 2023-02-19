package Utils

import (
	"crypto/sha256"
	"fmt"
	"io"
	"log"
	"os"
)

func Sha256(path string) string {
	f, err := os.Open(path)
	if err != nil {
		panic(err)
	}
	defer f.Close()

	hash := sha256.New()
	if _, err := io.Copy(hash, f); err != nil {
		log.Fatal(err)
	}
	sum := hash.Sum(nil)
	return fmt.Sprintf("%x\n", sum)
}
