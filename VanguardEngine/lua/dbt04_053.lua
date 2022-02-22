-- りーぷみゃー

function RegisterAbilities()
	-- on discard
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnDiscard)
	ability1.SetTrigger("OnDiscardTrigger")
	ability1.SetCost("OnDiscardCost")
	ability1.SetCanFullyResolve("OnDiscardCanFullyResolve")
	ability1.SetActivation("OnDiscard")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.RC)
	ability2.SetActivation("Cont")
end

function OnDiscardTrigger()
	return obj.IsPlayerTurn() and obj.IsApplicable()
end

function OnDiscardCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnDiscardCanFullyResolve()
	return obj.CanSuperiorCallToSpecificCircle({q.Other, o.ThisFieldID}, FL.BackRow)
end

function OnDiscard()
	obj.SuperiorCall({q.Other, o.ThisFieldID}, false, "", false, {FL.BackRow})
end

function Cont()
	if obj.Exists({q.Other, o.This, q.Other, o.Standing}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusSkills, s.Intercept, p.Continuous)
		obj.AddCardState({q.Other, o.This}, cs.CanInterceptFromBackRow, p.Continuous)
	end
end