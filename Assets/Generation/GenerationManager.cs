using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Generation
{
    class GenerationManager : MonoBehaviour
    {
        enum RuleSelectEnum { LISTORDER, RANDOM, PARALLEL };

        [SerializeField]
        bool hasGenerated = false;

        [SerializeField]
        bool generateOnStart = false;

        [SerializeField]
        List<Rule> Rules;

        [SerializeField]
        List<Meshes> Meshes;

        [SerializeField]
        Vector3Int GridToMapScale = new Vector3Int(1, 1, 1);

        [SerializeField]
        Shape AxiomShape;

        [SerializeField]
        RuleSelectEnum WhichRuleToChoose;

        int maxIter = 100;

        private readonly System.Random rnd = new System.Random();

        float timer = 0f;
        public float timeRegenerate = 3f;

        private async void Start()
        {
            if (!generateOnStart || hasGenerated)
                return;
            hasGenerated = true;
            StartGeneration(transform.position, transform.rotation);
            //combineMeshes();
        }

        private void Update()
        {
            if (!hasGenerated || timer > timeRegenerate)
            {
                timer = 0f;
                foreach (Transform child in transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                StartGeneration(transform.position, transform.rotation);
                hasGenerated = true;
            }
            timer += Time.deltaTime;
        }

        private void combineMeshes()
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            
            List<Material> materials = new List<Material>();
            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
                materials.AddRange( meshFilters[i].gameObject.GetComponent<MeshRenderer>().materials);
                i++;
            }
            transform.GetComponent<MeshFilter>().mesh = new Mesh();
            transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, false, true);
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.materials = materials.ToArray();
            transform.gameObject.SetActive(true);
        }

        public async void StartGeneration(Vector3 position, Quaternion rotation)
        {
            if (AxiomShape == null || AxiomShape.IsAxiom == false)
            {
                Debug.LogError("Unable to generate level. Axiom not valid");
                return;
            }
            var (results, succeed) = await Generate();
            if (!succeed)
                Debug.LogError("GenerateLevel failed");
            else
            {
                foreach (var shape in results)
                {
                    // Debug.Log(shape.Symbol + shape.Position.ToString());
                    if (shape.IsTerminal)
                        GenerateMeshForShape(shape, position, rotation);
                }

            }
        }

        public bool GenerateMeshForShape(Shape shape, Vector3 position, Quaternion rotation)
        {
            var mesh = Meshes.FirstOrDefault(m => m.Symbol == shape.Symbol);
            if (mesh == null)
            {
                Debug.LogError("No mesh scriptableObject for this symbol " + shape.Symbol);
                return false;
            }
            if (mesh.meshObjects.Count == 0)
            {
                Debug.LogError("No mesh prefab for this symbol " + shape.Symbol);
                return false;
            }
                
            var meshGameObj = mesh.meshObjects[rnd.Next() % mesh.meshObjects.Count];
            // var m = Instantiate(meshGameObj, position, rotation);
            // m.transform.position += shape.Position;
            //m.transform.position = Vector3.Scale(shape.Position, GridToMapScale);
            // m.transform.rotation = Quaternion.Euler(shape.Rotation.x + rotation.x,
            //     shape.Rotation.y + rotation.y, 
            //     shape.Rotation.z + rotation.z);
            var m = Instantiate(meshGameObj, transform);
            m.transform.SetPositionAndRotation(position + shape.Position, Quaternion.Euler(shape.Rotation.x + rotation.x,
                 shape.Rotation.y + rotation.y, 
                 shape.Rotation.z + rotation.z));
            return true;
        }

        public async Task<Tuple<List<Shape>, bool>> Generate()
        {
            if (AxiomShape == null)
                return Tuple.Create(new List<Shape>(), false);

            List<Shape> results = new List<Shape>();

            results.Add(AxiomShape);
            int i = 0;
            while (results.Count > 0)
            {
                i++;
                if (i > maxIter)
                {
                    Debug.LogError("More than maximum iterations, breaking. If not infinite loop, try increasing maxIter");
                    break;
                }
                    
                // check if results are all terminal symbols
                var nonTerminals = results.Where(res => !res.IsTerminal);
                var terminals = results.Where(res => res.IsTerminal);
                bool hasNonTerminal = nonTerminals.Count() > 0;
               
                if (!hasNonTerminal)
                    break;
                // select next shape to use as input
                Shape input = nonTerminals.ElementAt(0);

                // find matching rules (matching input shape symbol)
                var matchingRules = from rule in Rules where rule.PredecessorShape.Symbol == input.Symbol select rule;
                if (matchingRules.Count() == 0)
                {
                    Debug.LogError("No matching rule for shape " + input.Symbol.ToString());
                    return Tuple.Create(new List<Shape>(), false); 
                }
                    
                // randomly select rule or run all in parallel
                Rule ruleToUse;
                Tuple<List<Shape>, bool> output = Tuple.Create(new List<Shape>(), false);
                switch (WhichRuleToChoose)
                {
                    case RuleSelectEnum.LISTORDER:
                        ruleToUse = matchingRules.FirstOrDefault();
                        output = ruleToUse.CalculateRule(input);
                        break;
                    case RuleSelectEnum.RANDOM:
                        ruleToUse = matchingRules.ElementAt(rnd.Next(0, matchingRules.Count()));
                        output = ruleToUse.CalculateRule(input);
                        break;
                    case RuleSelectEnum.PARALLEL:
                        // TODO
                        List<Task<Tuple<List<Shape>, bool>>> ruleTasks = new List<Task<Tuple<List<Shape>, bool>>>();
                        foreach(var rule in matchingRules)
                        {
                            ruleTasks.Add(Task.FromResult(rule.CalculateRule(input)));
                        }
                        Task<Tuple<List<Shape>, bool>> outputTask = await Task.WhenAny(ruleTasks);
                        output  = outputTask.Result;
       
                        break;
                    default:
                        break;
                    

                }
                if (output != null)
                {
                    var (newShapes, succeed) = output;

                    if (!succeed)
                        return Tuple.Create(new List<Shape>(), false);
                    results = newShapes;
                }

                // update results
                nonTerminals = nonTerminals.Where(shape => shape != input);
                
                results.AddRange(terminals.ToList());
                results.AddRange(nonTerminals.ToList());
                

            }
            return Tuple.Create(results, true);
        }
    }
}
