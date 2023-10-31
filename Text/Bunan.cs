namespace Nonno.Text;

/*
 * H ─ 0:首 1:胴
 * * ┐
 * * ┴ 0:単 1:陪 2:長 3:特
 * * ┐
 * * ┤
 * * ┤
 * * ┤
 * L ┴ 象
 */
/*
 * | 無   | 日 Z | 月 X | 木 C | 火 V | 土 B | 金 N | 水 M | 〇 _ | 一 A | 二 S | 三 D | 四 F | 五 G | 六 H | 七 J |
 * | 八 K | 九 L | 甲 Q | 乙 W | 丙 E | 丁 R | 戊 T | 己 Y | 庚 U | 辛 I | 壬 O | 癸 P | 　   | 　   | 　   | 難 ` |
 * | 　   | 日 z | 月 x | 木 c | 火 v | 土 b | 金 n | 水 m | 　   | 一 a | 二 s | 三 d | 四 f | 五 g | 六 h | 七 j |
 * | 八 k | 九 l | 甲 q | 乙 w | 丙 e | 丁 r | 戊 t | 己 y | 庚 u | 辛 i | 壬 o | 癸 p | 天 . | 地 , | 人 ; | 難 @ |
 *
 * | 無   | 日 A | 月 B | 木 D | 火 F | 土 G | 金 C | 水 E | 〇 _ | 一 M | 二 Q | 三 Z | 四 R | 五 K | 六 Y | 七 J |
 * | 八 K | 九 P | 甲 W | 乙 N | 丙 O | 丁 T | 戊 I | 己 S | 庚 U | 辛 L | 壬 H | 癸 X | 　   | 　   | 　   | 難 ` |
 * | 　   | 日 a | 月 b | 木 d | 火 f | 土 g | 金 c | 水 e | 　   | 一 m | 二 q | 三 z | 四 r | 五 k | 六 y | 七 j |
 * | 八 k | 九 p | 甲 w | 乙 n | 丙 o | 丁 t | 戊 i | 己 s | 庚 u | 辛 l | 壬 h | 癸 x | 天 . | 地 , | 人 ; | 難 @ |
 *
 * | 無   | 水 E | 金 C | 土 G | 火 F | 木 D | 月 B | 日 A | 〇 _ | 一 M | 二 Q | 三 Z | 四 R | 五 K | 六 Y | 七 J |
 * | 八 K | 九 P | 癸 X | 壬 H | 辛 L | 庚 U | 己 S | 戊 I | 丁 T | 丙 O | 乙 N | 甲 W | 　   | 　   | 　   | 難 ` |
 * | 　   | 水 e | 金 c | 土 g | 火 f | 木 d | 月 b | 日 a | 　   | 一 m | 二 q | 三 z | 四 r | 五 k | 六 y | 七 j |
 * | 八 k | 九 p | 癸 x | 壬 h | 辛 l | 庚 u | 己 s | 戊 i | 丁 t | 丙 o | 乙 n | 甲 w | 地 , | 人 ; | 天 . | 難 @ |
 *
 * | 無   | 天   | 丙   | 甲   | 庚   | 戊   | 日   | 壬   | 癸   | 月   | 己   | 辛   | 乙   | 丁   | 地   | 人   |
 * | 〇 _ | 一 A | 二 S | 三 D | 四 F | 五 G | 六 H | 七 J | 八 K | 九 L | 木 C | 火 V | 土 B | 金 N | 水 M | 難 @ |
 * 
 * |000000|000001|000010|000011|000100|000101|000110|000111|001000|001001|001010|001011|001100|001101|001110|001111|
 * |010000|010001|010010|010011|010100|010101|010110|010111|011000|011001|011010|011011|011100|011101|011110|011111|
 * |100000|100001|100010|100011|100100|100101|100110|100111|101000|101001|101010|101011|101100|101101|101110|101111|
 * |110000|110001|110010|110011|110100|110101|110110|110111|111000|111001|111010|111011|111100|111101|111110|111111|
 */
public readonly struct Bunan : IDirector<IBunanWeaver>
{
    readonly byte _code;

    public char Char
    {
        get => (_code & 0b11111) switch
        {
            0 => '無',
            1 => '日',
            2 => '月',
            3 => '木',
            4 => '火',
            5 => '土',
            6 => '金',
            7 => '水',
            8 => '〇',
            9 => '一',
            10 => '二',
            11 => '三',
            12 => '四',
            13 => '五',
            14 => '六',
            15 => '七',
            16 => '八',
            17 => '九',
            18 => '甲',
            19 => '乙',
            20 => '丙',
            21 => '丁',
            22 => '戊',
            23 => '己',
            24 => '庚',
            25 => '辛',
            26 => '壬',
            27 => '癸',
            28 => '天',
            29 => '地',
            30 => '人',
            31 or _ => '難',
        };
    }
    public byte Code => _code;
    public int Value => _code & 0b11111;
    public bool IsSeparator => _code == default;

    public Bunan(byte code)
    {
        _code = code;
    }

    public override string ToString()
    {
        return Char.ToString();

        //return ((_value & 0b100000) == 0, _value >> 6) switch
        //{
        //    (true, 0) => $"{c}",
        //    (false, 0) => $"({c})",
        //    (true, 1) => $"{c}.",
        //    (false, 1) => $"({c}).",
        //    (true, 2) => $"{c},",
        //    (false, 2) => $"({c}),",
        //    (true, _) => $"{c};",
        //    (false, _) => $"({c});",
        //};
    }

    public void Direct(IBunanWeaver target) => target.Pose(this);

    public static implicit operator int(Bunan code) => code.Value;
    public static implicit operator byte(Bunan code) => (byte)code.Value;
    public static implicit operator Bunan(byte @byte) => new(@byte);
}
