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


tito hash pseudokodas:

function Mixing(input, output_size = 32, salt = "")

    data = concatenate(input, salt)

    output = array of bytes length output_size initialized to 0

    for i from 0 to length(data) - 1
        ch = byte value of data[i]

        for j from 0 to output_size - 1
            output[j] = output[j] XOR ((ch + j * 13) mod 256)
            output[j] = rotate_left(output[j], 3 bits, 8-bit size)
            output[j] = (output[j] + ch + i) mod 256
        end for
    end for

    // convert byte array to string representation
    result_chars = array of chars length (output_size * 2)

    for i from 0 to output_size - 1
        b = output[i] >> 4
        result_chars[i * 2] = hex_char(b)

        b = output[i] AND 0x0F
        result_chars[i * 2 + 1] = hex_char(b)
    end for

    return string(result_chars)
end function


function hex_char(nibble)

    if nibble < 10
        return character('0' + nibble)
    else
        return character('A' + (nibble - 10))
    end function
    return res

titoAI pseudokodas:


function StormHash.ComputeHash(input)

    if input is null then input = ""
    data = UTF8Bytes(input)

    hash[0..7] = INITIAL_HASH[0..7]

    inputLength = length(data)
    hash[0] = hash[0] XOR inputLength
    hash[7] = hash[7] XOR (inputLength << 32)

    fullChunks = inputLength / 64
    for chunk from 0 to fullChunks-1
        ProcessChunk(data, chunk * 64, hash)

    remainingBytes = inputLength mod 64
    if remainingBytes > 0 or inputLength == 0
        ProcessFinalChunk(data, fullChunks * 64, remainingBytes, hash)

    FinalMix(hash)

    compressed = CompressTo256(hash)
    return HashToHexString(compressed)
end function


function ProcessChunk(data, offset, hash)

    words[0..7] = 0
    for i from 0 to 7
        words[i] = BytesToULong(data, offset + i*8)

    repeat 4 times (round = 0..3)
        MixingRound(hash, words, round)
end function


function ProcessFinalChunk(data, offset, remainingBytes, hash)

    finalChunk[64] = all zero
    if remainingBytes > 0
        copy remainingBytes from data[offset..] into finalChunk[0..]

    finalChunk[remainingBytes] = 0x80

    bitLength = length(data) * 8
    write bitLength as 8 bytes into finalChunk[56..63]

    ProcessChunk(finalChunk, 0, hash)
end function


function MixingRound(hash, words, round)

    temp[0..7] = hash[0..7]

    for i from 0 to 7
        next = (i + 1) mod 8
        prev = (i + 7) mod 8
        mixed = words[i]

        switch round
            case 0: mixed = RotateLeft(mixed XOR PRIME1, 31) * PRIME2
            case 1: mixed = RotateLeft(mixed + PRIME3, 17) XOR PRIME4
            case 2: mixed = (mixed * PRIME5) XOR RotateLeft(mixed, 23)
            case 3: mixed = RotateLeft(mixed XOR PRIME1, 13) + PRIME2

        temp[i] = hash[i] XOR mixed XOR RotateLeft(hash[next], 7) XOR RotateLeft(hash[prev], 25)
        temp[i] = RotateLeft(temp[i], 11) * PRIME1

    for i from 0 to 7
        hash[i] = temp[i] XOR temp[(i+3) mod 8] XOR temp[(i+5) mod 8]
end function


function FinalMix(hash)

    repeat 5 times (round = 0..4)
        for i from 0 to 7
            hash[i] = hash[i] XOR hash[(i+1) mod 8]
            hash[i] = RotateLeft(hash[i], 19) * PRIME1
            hash[i] = hash[i] XOR (hash[i] >> 17)
            hash[i] = hash[i] * PRIME3
            hash[i] = hash[i] XOR (hash[i] >> 13)
            hash[i] = hash[i] * PRIME5
            hash[i] = hash[i] XOR (hash[i] >> 16)

        if round < 4
            temp = hash[0]
            for i from 0 to 6
                hash[i] = hash[i] XOR hash[i+1]
            hash[7] = hash[7] XOR temp
end function


function BytesToULong(data, offset)

    result = 0
    for i from 0 to 7
        if offset + i < length(data)
            result = result OR (data[offset+i] << (8*i))
    return result
end function


function RotateLeft(value, bits)

    return (value << bits) OR (value >> (64 - bits))
end function


function CompressTo256(state)

    out[0..3] = 0
    for i from 0 to 3
        out[i] = state[i] XOR RotateLeft(state[i+4], (i*13) mod 64) XOR (PRIME3 + i*0x9E)
        out[i] = out[i] XOR (out[i] >> 23)
        out[i] = out[i] * PRIME2
        out[i] = out[i] XOR RotateLeft(out[i], 41)
    return out
end function


function HashToHexString(hash)

    string = ""
    for each value in hash
        append value formatted as 16 hex digits to string
    return string
end function

