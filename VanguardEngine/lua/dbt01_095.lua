-- Actual Analyst Kokabiel

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.GC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.GC, q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Grade, 3
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsGuardian() then
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
	local count = 0
	if n == 1 then
		if obj.VanguardIs("Apex Ruler, Bastion") then
			count = obj.GetNumberOf(2)
			if count % 2 > 0 then
				count = count - 1
			end
			count = count / 2
			obj.SetAbilityShield(1, count * 5000)
		end
	end
	return 0
end