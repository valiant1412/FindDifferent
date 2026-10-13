# Garden spot-the-difference pair

Generated using the built-in image_gen tool.

## Answer key (A -> B)
1. Uppermost apple: red -> yellow.
2. Round attic window glass: blue -> purple.
3. Watering can: blue -> pink.
4. Butterfly wings: red/orange -> blue.
5. Highest flower in foreground pot: pink petals -> white petals.

Both images were visually inspected. These are the five intended differences; generative editing may also introduce small rendering variations.

## Original prompt

Use case: illustration-story. Asset type: original image A for a spot-the-difference mobile game. Create ONE square 1024x1024 image, not a pair or collage. A charming sunny cottage garden, clean polished 2D cartoon illustration with crisp dark outlines, flat pastel colors and very restrained shading, easy to inspect on a phone. Balanced scene: small cream cottage with terracotta roof in upper center, round blue window above a brown door; tree at left with exactly three prominent red apples; white picket fence across background; one large yellow sun at upper right and two small white clouds. Foreground: an orange cat sitting on path near center, blue watering can at lower left, a terracotta pot with three pink flowers at lower right, one red butterfly hovering above the pot. Green grass with a few simple flower tufts. All objects fully visible and spatially separated. No writing, no UI, no watermark, no borders, no texture noise. These five details must be clear and isolated for later local changes: round blue window, three apples, yellow sun, blue watering can, red butterfly. This is the base picture; do not create differences or a second panel.

## Edit prompt

Use case: precise-object-edit. Asset type: image B for a spot-the-difference game. Input image is the exact edit target, image A. Create ONE matching square image with EXACTLY FIVE localized differences. Keep original framing, dimensions, shapes, outlines, camera, all object positions, every background detail and all unlisted colors unchanged. Do not redraw or reinterpret the scene. Changes: (1) recolor only the uppermost red apple at the upper left of the tree to bright yellow, keeping its stem, leaves and shape; other two apples stay red. (2) recolor the blue glass panes in the round attic window to lavender purple, keeping the round frame and crossbars identical. (3) recolor the large blue watering can at bottom left to pink, preserving its outlines, holes, highlights, shape and position. (4) recolor only the red/orange areas of the butterfly wings above the flowerpot to sky blue, keeping black outlines, spots and its shape. (5) recolor only the pink petals of the highest, middle flower in the foreground pot to white, keeping yellow center and the other two flowers pink. Strict invariants: the sun, clouds, tree foliage, house roof and walls, all other windows, door, lamp, fence, cat, grass, garden flowers, paths and plant pot remain exactly as in the source. No added objects, no removal of objects, no text, no labels, no circles, no borders. Only these five local color substitutions.

