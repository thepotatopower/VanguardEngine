-- ユースベルク "反抗黎順・紅蓮"

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
	ability1.SetActivation("OnPlace")
	-- end of turn
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnEndPhase)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("OnEndPhaseTrigger")
	ability2.SetCondition("OnEndPhaseCondition")
	ability2.SetActivation("OnEndPhase")
end

function OnPlaceTrigger()
	return obj.IsApplicable() and obj.SourceHasProperty(p.RevolDress)
end

function OnPlaceCondition()
	return obj.IsSameZone()
end

function OnPlace()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 15000, p.UntilEndOfTurn)
	if obj.PersonaRode() then
		obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusCritical, 1, p.UntilEndOfTurn)
	end
end

function OnEndPhaseTrigger()
	return obj.IsPlayerTurn()
end

function OnEndPhaseCondition()
	return obj.CanSuperiorRide("Filter", {l.Soul})
end

function Filter(id)
	return obj.HasProperty(id, p.RevolDress)
end

function OnEndPhase()
	obj.Select("Filter", {l.Soul}, 1, 1, Prompt.Ride)
	obj.SuperiorRide({q.Location, l.Selected}, false)
end