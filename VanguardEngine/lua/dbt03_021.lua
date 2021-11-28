-- 雲上の天激 ブラガー

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable() 
end

function Condition()
	return obj.HandCount() <= 4
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	obj.LookAtTopOfDeck(5)
	obj.SuperiorCall({q.Location, l.Looking, q.Count, 1, q.Min, 0})
	obj.Shuffle()
end