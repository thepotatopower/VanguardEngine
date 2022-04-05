-- 禁令の邪眼 クェン・ルゥ

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.PlayerRC, q.Other, o.NotThis})
end

function Cost(check)
	if check then return obj.CanAddToSoul({q.Location, l.PlayerRC, q.Other, o.This}) end
	obj.AddToSoul({q.Location, l.PlayerRC, q.Other, o.This})
end

function Activation()
	obj.Select({q.Location, l.PlayerRC, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 2000, p.UntilEndOfTurn)
	if obj.SoulCount() >= 7 and obj.GetNumberOf({q.Location, l.Selected}) > 0 then
		local id = obj.GetID({q.Location, l.Selected})
		local timedTrigger = GiveAbility(GetID(), id)
		timedTrigger.SetTiming(a.Cont)
		timedTrigger.SetActivation("TimedTrigger")
		timedTrigger.SetResetTarget(id)
		timedTrigger.SetResetTiming(p.UntilEndOfTurn)
	end
end

function TimedTrigger()
	if obj.IsAttackingUnit() then
		obj.AddPlayerValue(ps.MinGradeForGuard, 0, p.Continuous, false)
	end
end