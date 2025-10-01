Musų hasho stiprybės ir silpnybės:

Kolizijos:
Visi hash'ai turi 0 kolizijų su darytais testais

Lavinos efektas:
gab hash rodo gerus rezultatus (67% skirtumas vidutiniškai)
tito ir titoAI hashai rodo įspudingus rezultatus (93% skirtumas), tiek pat kiek SHA256

Greitis:
gab hash gerokai atsilieka greičio aspektu
tito hash yra keletą kartų greitesnis, tačiau vistiek gerokai atsilieka nuo visų likusių
titoAI hash dar keletą kartų greitesnis, tačiau vistiek nepasiekia populiariųjų hashų greičio



gab hashinimo funkcijos Pseudokodas:


FUNCTION HashPipeline(text, length):
    // Step 1: initial hash
    hashBytes = GetHashString(text, length)

    // Step 2: interpret as BigInteger
    bigInt = CONVERT_TO_BIG_INTEGER(hashBytes)

    // Step 3: multiply all bytes
    mult = MultNumbers(bigInt)

    // Step 4: hash the multiplication result again
    finalHash = GetHashString(TO_STRING(mult), length)

    RETURN finalHash


FUNCTION GetHashString(text, length):
    bytes = UTF8_ENCODE(text)

    WHILE size(bytes) < length * 2:
        shifted = ShiftBytes(bytes, 1)
        bytes = CONCAT(bytes, shifted)

    firstBytes = TAKE(bytes, length)
    bytes = SKIP(bytes, length)

    WHILE size(bytes) > 0:
        IF size(bytes) < length:
            padded = ZERO_ARRAY(length)
            COPY(bytes, padded)
            bytes = padded

        xorResult = XorArrays(firstBytes, TAKE(bytes, length))
        bytes = SKIP(bytes, length)
        firstBytes = xorResult

    RETURN firstBytes


FUNCTION XorArrays(a, b):
    length = MIN(size(a), size(b))
    result = NEW_ARRAY(length)

    FOR i FROM 0 TO length-1:
        result[i] = a[i] XOR b[i]

    RETURN result


FUNCTION ShiftBytes(bytes, shiftN):
    number = CONVERT_TO_BIG_INTEGER(bytes)
    shiftedLeft = number << shiftN
    shiftedBytes = CONVERT_TO_BYTE_ARRAY(shiftedLeft)

    result = CONCAT(shiftedBytes, bytes)
    RETURN result


FUNCTION MultNumbers(bigInt):
    res = 1
    bytes = CONVERT_TO_BYTE_ARRAY(bigInt)

    FOR each byte IN bytes:
        IF byte == 0:
            mult = 1
        ELSE:
            mult = byte

        res = res * mult

    RETURN res
