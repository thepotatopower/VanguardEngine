-- Grief, Despair, and Rejection

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then 
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 3
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
		if obj.VanguardIs("Mysterious Rain Spiritualist, Zorga") then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.VanguardIs("Mysterious Rain Spiritualist, Zorga") then
			obj.ChooseAddTempPower(1, 10000)
		end
	end
	return 0
end