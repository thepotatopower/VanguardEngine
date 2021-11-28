-- 焔の巫女 ヒメナ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable() 
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	obj.LookAtTopOfDeck(7)
	obj.DisplayCards({q.Location, l.Looking})
	obj.Search({q.Location, l.Looking, q.UnitType, u.overDress, q.Count, 1})
end