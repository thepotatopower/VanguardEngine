-- Howling Ballad, Fanael

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Applicable, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.IsMandatory, p.BlackWings
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) then
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
		obj.SoulCharge(1)
		obj.AddSkillUntilEndOfTurn(1, s.Boost)
	end
	return 0
end