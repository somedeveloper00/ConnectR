using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private bool isAi;
    private float boardStateUtility; // value for the current board state used in min & max choices
    private int bestMoveSeenSoFar; // Column that represents the best play based off board
    
    /*A	function	that	takes	in	a	state	and	generates	all	possible	states	given	the	valid
    moves/rules	of	the	game.		Hopefully	you	see	that	this	will	be	relatively	easy	given
        the	nature	of	the	“NxM	Connect-R”	game	we’re	using	in	this	project
    A utility	function	that	takes	in	a	state	and	player	and	generates	a	numeric	value
        denoting	the	quality	of	the	state	for	the	player.		For	example,	a -1	indicates	a	loss,	a
    +1	indicates	a	win,	and	every	value	in	between	is	a	qualitative	judgment	on	the	state
    of	the	board.
    • A	terminal	test	function	that	takes	in	a	state	and	returns	whether	the	game	is	over
        or	not.		Notice	that	if	you’ve	written the	utility	function	above,	the	terminal	test
    function	becomes	trivial.
    • The	recursive	min-max	function	itself.		See	page	166	in	the	course	text.*/

    public void DropCoin(int x)
    {
        
        // called via script no column click needed
        GameManager.instance.HandleColumnClick(x);
        HandleUserInput.handleUserInputSelection -= DropCoin;
    }

    public void GetPlayerInput()
    {
        // if not ai subscribe the callback for column selection
        if(!isAi)
            HandleUserInput.handleUserInputSelection += DropCoin;

        if (isAi)
        {
            DropCoin(GetActionFromMiniMax());
        }
    }

    private int GetActionFromMiniMax()
    {
        return Random.Range(0, GameManager.instance.GetXLength);
        
        
    }
}
