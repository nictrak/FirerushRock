using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureCatalog : MonoBehaviour
{
    //1.1
    public GameObject BigBed_up;
    public GameObject BigBed_left;
    public GameObject BigBed_right;
    //1.2
    public GameObject SmallBed_up;
    public GameObject SmallBed_left;
    public GameObject SmallBed_right;
    //1.3
    public GameObject Closet;
    //1.4, 3.2
    public GameObject BookShelf;
    //1.5
    public GameObject SmallChairAndTable_up;
    public GameObject SmallChairAndTable_left;
    public GameObject SmallChairAndTable_right;
    public GameObject SmallChairAndTable_down;

    //2.1
    public GameObject FlushToilet_up;
    public GameObject FlushToilet_left;
    public GameObject FlushToilet_right;
    //2.2
    public GameObject Bathtub_ver;
    public GameObject Bathtub_hor;
    //2.3
    public GameObject ShowerBox;
    //2.4
    public GameObject SinkAndMirror;

    //3.1
    public GameObject Chair22BigTable_ver;
    public GameObject Chair22BigTable_hor;
    //3.2 = 1.4
    //3.3
    public GameObject Plant;
    //3.4
    public GameObject SmallSofa_up;
    public GameObject SmallSofa_left;
    public GameObject SmallSofa_right;
    public GameObject SmallSofa_down;
    //3.5
    public GameObject LongSofa_up;
    public GameObject LongSofa_left;
    public GameObject LongSofa_right;
    public GameObject LongSofa_down;
    //3.6
    public GameObject TwoSofaAndLongSofaAndTable_up;
    public GameObject TwoSofaAndLongSofaAndTable_left;
    public GameObject TwoSofaAndLongSofaAndTable_right;
    public GameObject TwoSofaAndLongSofaAndTable_down;
    //3.7
    public GameObject TwoChairAndTable_ver;
    public GameObject TwoChairAndTable_hor;
    //3.8
    public GameObject TwoSofaAndRoundTable_ver;
    public GameObject TwoSofaAndRoundTable_hor;
    //3.9
    public GameObject Drawer;


    //4.1-4.6
    public GameObject Wall_1;
    public GameObject Wall_2;
    public GameObject Wall_3;
    public GameObject Wall_4;
    public GameObject Wall_5;
    public GameObject Wall_6;

    //5.1-5.4
    public GameObject Survivor_1;
    public GameObject Survivor_2;
    public GameObject Survivor_3;
    public GameObject Survivor_4;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Furniture(int furnitureID)
    {
        switch (furnitureID)
        {
            //bedroom
            case 1011: return BigBed_up;
            case 1012: return BigBed_left;
            case 1013: return BigBed_right;

            case 1021: return SmallBed_up;
            case 1022: return SmallBed_left;
            case 1023: return SmallBed_right;

            case 1031: return Closet;

            case 1041: return BookShelf;

            case 1051: return SmallChairAndTable_up;
            case 1052: return SmallChairAndTable_left;
            case 1053: return SmallChairAndTable_right;
            case 1054: return SmallChairAndTable_down;

            //toilet
            case 2011: return FlushToilet_up;
            case 2012: return FlushToilet_left;
            case 2013: return FlushToilet_right;

            case 2021: return Bathtub_ver;
            case 2022: return Bathtub_hor;

            case 2031: return ShowerBox;

            case 2041: return SinkAndMirror;

            //livingroom
            case 3011: return Chair22BigTable_ver;
            case 3012: return Chair22BigTable_hor;

            case 3021: return BookShelf;

            case 3031: return Plant;

            case 3041: return SmallSofa_up;
            case 3042: return SmallSofa_left;
            case 3043: return SmallSofa_right;
            case 3044: return SmallSofa_down;

            case 3051: return LongSofa_up;
            case 3052: return LongSofa_left;
            case 3053: return LongSofa_right;
            case 3054: return LongSofa_down;

            case 3061: return TwoSofaAndLongSofaAndTable_up;
            case 3062: return TwoSofaAndLongSofaAndTable_left;
            case 3063: return TwoSofaAndLongSofaAndTable_right;
            case 3064: return TwoSofaAndLongSofaAndTable_down;

            case 3071: return TwoChairAndTable_ver;
            case 3072: return TwoChairAndTable_hor;

            case 3081: return TwoSofaAndRoundTable_ver;
            case 3082: return TwoSofaAndRoundTable_hor;

            case 3091: return Drawer;

            //wall furniture
            case 41: return Wall_1;
            case 42: return Wall_2;
            case 43: return Wall_3;
            case 44: return Wall_4;
            case 45: return Wall_5;
            case 46: return Wall_6;

            //survivor
            case 51: return Survivor_1;
            case 52: return Survivor_2;
            case 53: return Survivor_3;
            case 54: return Survivor_4;
        }
        return Survivor_1;
    }
}
