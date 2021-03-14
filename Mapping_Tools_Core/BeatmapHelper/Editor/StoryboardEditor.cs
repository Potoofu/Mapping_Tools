﻿using Mapping_Tools_Core.BeatmapHelper.Parsing;

namespace Mapping_Tools_Core.BeatmapHelper.Editor {
    /// <summary>
    /// Editor specifically for storyboards
    /// </summary>
    public class StoryboardEditor : Editor<StoryBoard> {
        public StoryboardEditor() : base(new OsuStoryboardParser()) {}

        public StoryboardEditor(string path) : base(new OsuStoryboardParser(), path) {}
    }
}
