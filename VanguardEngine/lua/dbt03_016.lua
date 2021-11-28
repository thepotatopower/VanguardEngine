-- デザイアデビル インケーン

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
	ability1.SetActivation("OnPlace")
	-- on put into soul from RC
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnPut)
	ability2.SetMovedFrom(l.RC)
	ability2.SetMovedTo(l.Soul)
	ability2.SetTrigger("OnPutTrigger")
	ability2.SetCost("OnPutCost")
	ability2.SetActivation("OnPut")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.PersonaRode()
end

function OnPlace()
	obj.SoulCharge(1)
end

function OnPutTrigger()
	return obj.IsApplicable() and obj.SourceIsVanguardAbility()
end

function OnPutCost(check)
	if check then return obj.CanCB(2) end
	obj.CounterBlast(2)
end

function OnPut()
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusDrive, 1, p.UntilEndOfTurn)
end