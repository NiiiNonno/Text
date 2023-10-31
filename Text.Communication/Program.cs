using Nonno.Text;
using Nonno.Text.Communication;

var token = "VLwoxzCBh4ilAtgoKoHFp3XumU4j5YITtAoS5TqWLT3";
var lc = new LineNotifyForum() { Token = "qnpBN0xvzSHb3hczdapm7ILLVWeIqKqm8UtgemSwjl7" };
await lc.Send("test");

var t = new UTF16Text();
t.Add<Bunan, IBunanWeaver>(new Bunan());