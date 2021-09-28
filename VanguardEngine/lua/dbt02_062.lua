-- Blaze Fist Monk, Enten

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 1, q.Other, o.NotThis
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttackHits, p.HasPrompt, p.SB, 1, p.Retire, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsBooster() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(2) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(2, 10000)
	end
	return 0
end