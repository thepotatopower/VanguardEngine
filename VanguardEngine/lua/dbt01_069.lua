-- Selfish Engraver

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Damage, q.Other, o.FaceDown, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttackHits, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.OnBattleEnds, p.HasPrompt, p.AddToSoul, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.SoulCount() >= 10 then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.Exists(2) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.SoulCharge(1)
	elseif n == 2 then
		obj.CounterCharge(1)
	end
	return 0
end