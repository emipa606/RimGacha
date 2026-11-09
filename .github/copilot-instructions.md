# GitHub Copilot Instructions for RimGacha (Continued)

## Mod Overview and Purpose

RimGacha is an update to Lanilors original mod, bringing gacha games to the RimWorld game. Players can collect hundreds of gacha figures, hunt for rare ones, and arrange them in decorative collections. The mod adds three stats to gacha figures: animal design, material, and quality, defining their rarity, value, base beauty, and possible set bonuses.

Key features include:
- Hundreds of gacha figures to collect
- Rarity and value calculation for all animals and materials from other mods
- Built-in mod support for all animal and material mods
- Decorative collections for beauty boost
- Jurassic Rimworld support for dinosaur figures
- Can be added to existing saves

## Key Features and Systems

### Gacha Figures
- Collect and arrange gacha figures in decorative collections.
- Gacha figures have three stats: animal design, material, and quality.
- Rarity and value calculation for existing figures and new additions.

### Decorative Collections
- Create collections to showcase your gacha figures and receive beauty boosts.
- Collections can be arranged in various ways for aesthetic appeal.

### Mod Support
- Built-in support for all animal and material mods.
- Automatically adds new animals and materials to the pool of gacha figures.
- Recalculates rarity and values when adding new animals or materials.

### Jurassic Rimworld
- Compatible with Jurassic Rimworld mod for dinosaur figures.

### Existing Saves
- Can be added to existing saves without losing progress.

### Performance Optimization
- Textures for gacha figures are generated only when needed to save memory and loading times.
- Tiny lags may occur when a specific figure first appears in the game, depending on CPU power.

## Coding Patterns and Conventions

### C# Files
- **Building_Art_Gachapon.cs**: Defines the building for gacha figures.
- **HarmonyPatches/***: Contains Harmony patches for various game systems to integrate gacha functionality.

### XML Files
- **RecipeDefs/Recipes_Misc.xml**: Defines recipes related to gacha figures.
- **ThingDefs_Buildings/***: Defines building and furniture related to gacha figures.
- **WorkGiverDefs/WorkGivers.xml**: Defines work givers related to gacha figures.

## XML Integration

RimGacha uses XML definitions to integrate various aspects of the mod into RimWorld. The following XML files are included:

- **Recipes_Misc.xml**: Contains recipe definitions for gacha figure-related recipes.
- **Buildings_Art.xml, Buildings_Furniture.xml, Buildings_Misc.xml**: Contains thing definitions for gacha figure-related buildings and furniture.
- **WorkGivers.xml**: Contains work giver definitions for gacha figure-related work.

## Harmony Patching

RimGacha uses Harmony to patch various RimWorld methods to integrate gacha functionality. The following Harmony patches are included:

- **Designator_Install_DrawGhost.cs**: Prefix method for `Designator_Install_DrawGhost.Prefix`.
- **GenConstruct_BlocksConstruction.cs**: Prefix method for `GenConstruct_BlocksConstruction.Prefix`.
- **GenConstruct_CanPlaceBlueprintOver.cs**: Prefix method for `GenConstruct_CanPlaceBlueprintOver.Prefix`.
- **GenRecipe_PostProcessProduct.cs**: Prefix method for `GenRecipe_PostProcessProduct.Prefix`.

## Suggestions for Copilot

### General Guidelines
- Follow RimWorld's coding conventions and best practices.
- Use Harmony patches for mod integration where necessary.
- Ensure XML definitions are correctly formatted and integrated.

### Specific Recommendations
- Use Copilot to generate and integrate Harmony patches for new game systems or functionality.
- Use Copilot to create new XML definitions for buildings, recipes, or work givers.
- Use Copilot to generate new C# classes for gacha figure-related systems or collections.

### Examples
- **Harmony Patch Example**:
  csharp
  [HarmonyPatch(typeof(Designator_Install_DrawGhost), "Prefix")]
  public static class Designator_Install_DrawGhost_Prefix
  {
      public static void Prefix(Designator_Install_DrawGhost __instance)
      {
          // Custom logic for gacha figure installation
      }
  }
  

- **XML Definition Example**:
  xml
  <RecipeDef>
      <defName>GachaFigureRecipe</defName>
      <label>Make Gacha Figure</label>
      <description>Makes a new gacha figure.</description>
      <workAmount>100</workAmount>
      <products>
          <li>
              <thingDef>GachaFigure</thingDef>
              <count>1</count>
          </li>
      </products>
      <ingredients>
          <li>
              <thingDef>Wood</thingDef>
              <count>1</count>
          </li>
      </ingredients>
  </RecipeDef>
  

- **C# Class Example**:
  csharp
  public class GachaCollection
  {
      public List<GachaFigure> Figures { get; set; } = new List<GachaFigure>();

      public void AddFigure(GachaFigure figure)
      {
          Figures.Add(figure);
      }

      public void RemoveFigure(GachaFigure figure)
      {
          Figures.Remove(figure);
      }
  }
  

By following these guidelines and recommendations, you can effectively use GitHub Copilot to enhance your work on RimGacha (Continued).

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

