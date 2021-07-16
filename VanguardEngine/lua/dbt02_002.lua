-- Vairina Erger

function NumberOfAbilities()
	return 3
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Name, "Trickstar"
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Name, "Trickstar", q.Other, o.Attacking, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Soul, q.Count, 2
	elseif n == 4 then
		return q.Location, l.PlayerRC, q.Location, l.GC, q.Other, o.This
	elseif n == 5 then
		return q.Location, l.PlayerHand, q.Other, o.This, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OverDress, 1
	elseif n == 2 then
		return a.OnBattleEnds, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.SB, 2
	elseif n == 3 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 2 then
		if obj.Exists(5) and obj.Exists(2) and obj.TargetIsEnemyVanguard() and obj.VanguardIs("Chakrabarthi Divine Dragon, Nirvana") and obj.CanSB(3) then
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

function Cost(n)
	if n == 2 then
		obj.SoulBlast(3)
	end
end

function Activate(n)
	if n == 2 then
		obj.SuperiorOverDress(5, 2)
		obj.CounterCharge(1)
	elseif n == 3 then
		if obj.InOverDress() then
			obj.SetAbilityPower(4, 10000)
			obj.SetAbilityShield(4, 10000)
		else
			obj.SetAbilityPower(4, 0)
			obj.SetAbilityShield(4, 0)
		end
	end
	return 0
end
