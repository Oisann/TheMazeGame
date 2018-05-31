using UnityEngine;
using UnityEngine.UI;

public class MapRoom : MonoBehaviour {
    public class showQ {
        public bool n = true;
        public bool s = true;
        public bool e = true;
        public bool w = true;
    }

    public RoomController rc;
    public int x, y;
	public Sprite[] icons;
    public showQ questionMarks = new showQ();

    private GameObject n, s, e, w, qN, qS, qE, qW, icon;
	private Image iconSprite;
    
    [HideInInspector]
    public int roomGap = 0;

    private RectTransform rect;
    private MapController mc;
    private bool registered = false;

	void Start() {
        rect = GetComponent<RectTransform>();
        mc = transform.parent.GetComponentInParent<MapController>();

        n = transform.GetChild(0).GetChild(0).gameObject;
        s = transform.GetChild(0).GetChild(1).gameObject;
        e = transform.GetChild(0).GetChild(2).gameObject;
        w = transform.GetChild(0).GetChild(3).gameObject;

        qN = n.transform.GetChild(0).gameObject;
        qS = s.transform.GetChild(0).gameObject;
        qE = e.transform.GetChild(0).gameObject;
        qW = w.transform.GetChild(0).gameObject;

        icon = transform.GetChild(0).GetChild(4).gameObject;
		iconSprite = icon.GetComponent<Image>();

        n.SetActive(false);
        s.SetActive(false);
        e.SetActive(false);
        w.SetActive(false);

        qN.SetActive(questionMarks.n);
        qS.SetActive(questionMarks.s);
        qE.SetActive(questionMarks.e);
        qW.SetActive(questionMarks.w);

        icon.SetActive(false);

        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Update() {
        if(!registered) {
            if(mc.isReady()) {
                mc.SetRoom(x, y, rc, this);
                rect.anchoredPosition = mc.getRoomScreenPos(x, y);
                registered = true;
            }
        }
    }

    public void FixedUpdate() {
        if(!registered)
            return;

		if(rc.isStart && iconSprite.sprite != icons[0]) {
			iconSprite.sprite = icons[0];
		}

		if(rc.isEnd && iconSprite.sprite != icons[1]) {
			iconSprite.sprite = icons[1];
		}

		if(rc.puzzleComplete && iconSprite.sprite != icons[2]) {
			iconSprite.sprite = icons[2];
		} 

		if(rc.isStart && rc.isStart != icon.activeInHierarchy)
            icon.SetActive(rc.isStart);

		if(rc.isEnd && rc.isEnd != icon.activeInHierarchy)
			icon.SetActive(rc.isEnd);

		if(rc.puzzleComplete && rc.puzzleComplete != icon.activeInHierarchy)
			icon.SetActive(rc.puzzleComplete);

        if(rc.displayRoom != transform.GetChild(0).gameObject.activeInHierarchy) {
            transform.GetChild(0).gameObject.SetActive(rc.displayRoom);
            mc.UpdateRoom(x, y);
        }
        if(rc.hasDoorNorth != n.activeInHierarchy)
            n.SetActive(rc.hasDoorNorth);
        if(rc.hasDoorSouth != s.activeInHierarchy)
            s.SetActive(rc.hasDoorSouth);
        if(rc.hasDoorEast != e.activeInHierarchy)
            e.SetActive(rc.hasDoorEast);
        if(rc.hasDoorWest != w.activeInHierarchy)
            w.SetActive(rc.hasDoorWest);

        if(questionMarks.n != qN.activeInHierarchy)
            qN.SetActive(questionMarks.n);
        if(questionMarks.s != qS.activeInHierarchy)
            qS.SetActive(questionMarks.s);
        if(questionMarks.e != qE.activeInHierarchy)
            qE.SetActive(questionMarks.e);
        if(questionMarks.w != qW.activeInHierarchy)
            qW.SetActive(questionMarks.w);
    }
}
