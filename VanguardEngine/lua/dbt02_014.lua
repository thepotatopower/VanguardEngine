-- Cleave Muddler

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerHand, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.Discard, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.VanguardIs("Master of Gravity, Baromagnes") and obj.Exists(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.Discard(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.AddBattleOnlyPower(2, 5000)
		if obj.SoulCount() >= 10 then
			obj.CounterCharge(1)
		end
	end
	return 0
end
