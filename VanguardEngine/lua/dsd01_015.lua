-- Sunburst Evolution

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Drop, q.Name, "Vairina", q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, p.CB, 1
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
		if obj.Exists(2) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(1, 5000)
		obj.ChooseAddToHand(2)
	end
	return 0
end