-- Vairina Erger

function NumberOfAbilities()
	return 3
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Name, "Trickstar"
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Name, "Trickstar", q.Other, o.Attacking, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Location, l.GC, q.Other, o.This
	elseif n == 4 then
		return q.Location, l.PlayerHand, q.Other, o.This, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OverDress, 1
	elseif n == 2 then
		return a.OnBattleEnds, t.Auto, p.HasPrompt, p.SB, 2
	elseif n == 3 then
		return a.Cont, t.Cont, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 2 then
		if obj.Exists(4) and obj.Exists(2) and obj.TargetIsEnemyVanguard() and obj.VanguardIs("Chakrabarthi Divine Dragon, Nirvana") then
			return true
		end
	elseif n == 3 then
		if obj.IsRearguard() or obj.IsGuardian() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	elseif n == 3 then
		return true
	end
	return false
end

function Activate(n)
	if n == 2 then
		obj.SuperiorOverDress(4, 2)
		obj.CounterCharge(1)
	elseif n == 3 then
		if obj.InOverDress() then
			obj.SetAbilityPower(3, 10000)
			obj.SetAbilityShield(3, 10000)
		end
	end
	return 0
end
