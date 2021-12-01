-- 響き合う希望の祈り

function RegisterAbilities()
	-- order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	local id = obj.GetID({q.Location, l.Selected})
	local given = GiveAbility(GetID(), id)
	given.SetDescription(2)
	given.SetTiming(a.PlacedOnRC)
	given.SetLocation(l.VC)
	given.SetTrigger("OnPlaceTrigger")
	given.SetCondition("OnPlaceCondition")
	given.SetActivation("OnPlace")
	given.SetResetTarget(id)
	given.SetResetTiming(p.UntilEndOfTurn)
end

function OnPlaceTrigger()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.Player})
end

function OnPlaceCondition()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.Player, q.Other, o.OverDress})
end

function OnPlace()
	obj.Draw(1)
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end