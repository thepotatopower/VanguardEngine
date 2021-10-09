-- Scramble Sprint, Selma

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsBooster() and obj.VanguardIsAttackingUnit() then
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

function Activate(n)
	if n == 1 then
		obj.Draw(1)
		obj.Discard(1)
	end
	return 0
end