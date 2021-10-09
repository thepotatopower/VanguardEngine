-- Friend Up, Ilda

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Applicable, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.NameContains, "Earnescorrect", q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PutOnGC, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) and obj.Exists(2) then
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
		obj.ChooseAddToHand(2)
	end
	return 0
end