-- お助け怪獣 テクタン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnDiscard)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsPlayerTurn() and obj.IsApplicable()
end

function Activation()
	obj.LookAtTopOfDeck(5)
	obj.Search({q.Location, l.Looking, q.Other, o.SetOrder, q.Count, 1, q.Min, 0})
end