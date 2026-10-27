#ifndef TRIPLANAR_TWO_AXIS_INCLUDED
#define TRIPLANAR_TWO_AXIS_INCLUDED

// Two-axis triplanar projection.
//
// Shader Graph's Triplanar node always takes three samples and weights them by
// normalised abs(normal). The smallest of those three weights is usually tiny, so
// this drops that axis entirely and renormalises the remaining two - two samples
// instead of three, for a result that is visually very close.
//
// The branch only selects UV coordinates; both SAMPLE calls run unconditionally,
// so there is no divergent-flow / missing-gradient problem.
//
// Matches Shader Graph's Triplanar with Blend = 1 (weights = abs(normal)) and an
// unconnected Sampler slot (uses the texture's own sampler state).
// Position and Normal are expected in world space, like the node's defaults.
//
// Two robustness notes, both of which matter at half precision:
//   - Interpolated normals are not unit length, so the input is normalised. Without
//     that, degenerate geometry can drive the weight sum towards zero.
//   - The divide guard is 1e-3, not 1e-5. Half's smallest normal is ~6.1e-5, so a
//     1e-5 guard is denormal and flushes to zero on many mobile GPUs - turning the
//     guard itself into a 0/0 and producing NaNs, which render as white speckles.
//     A NaN here survives even when the grid is later multiplied by zero, because
//     NaN * 0 is still NaN.

#define TRIPLANAR_TWO_AXIS_BODY(TYPE)                                            \
    TYPE##3 uv = Position * Tile;                                                \
    TYPE##3 n  = Normal;                                                         \
    TYPE nlen  = max(length(n), 1e-3);                                           \
    TYPE##3 w  = abs(n / nlen);                                                  \
                                                                                 \
    TYPE mn = min(w.x, min(w.y, w.z));                                           \
                                                                                 \
    TYPE##2 uvA, uvB, wt;                                                        \
    if (mn == w.x)                                                               \
    {                                                                            \
        uvA = uv.xz; uvB = uv.xy; wt = TYPE##2(w.y, w.z);                        \
    }                                                                            \
    else if (mn == w.y)                                                          \
    {                                                                            \
        uvA = uv.zy; uvB = uv.xy; wt = TYPE##2(w.x, w.z);                        \
    }                                                                            \
    else                                                                         \
    {                                                                            \
        uvA = uv.zy; uvB = uv.xz; wt = TYPE##2(w.x, w.y);                        \
    }                                                                            \
                                                                                 \
    wt /= max(wt.x + wt.y, 1e-3);                                                \
                                                                                 \
    Out = SAMPLE_TEXTURE2D(Tex.tex, Tex.samplerstate, uvA) * wt.x                \
        + SAMPLE_TEXTURE2D(Tex.tex, Tex.samplerstate, uvB) * wt.y;

void TriplanarTwoAxis_float(UnityTexture2D Tex,
                            float3 Position, float3 Normal, float Tile,
                            out float4 Out)
{
    TRIPLANAR_TWO_AXIS_BODY(float)
}

void TriplanarTwoAxis_half(UnityTexture2D Tex,
                           half3 Position, half3 Normal, half Tile,
                           out half4 Out)
{
    TRIPLANAR_TWO_AXIS_BODY(half)
}

#endif // TRIPLANAR_TWO_AXIS_INCLUDED
