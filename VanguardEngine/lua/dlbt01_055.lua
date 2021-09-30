-- Truehearted Ruby

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.OrderArea, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
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
		obj.Draw(2)
		obj.Discard(1)
		obj.AddToSoul(1)
	end
	return 0
end